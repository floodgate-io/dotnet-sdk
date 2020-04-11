using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FloodGate.SDK.Events
{
    public class EventProcessor : IEventProcessor, IDisposable
    {
        /// <summary>
        /// Maximum number of events that can be stored in the queue before it's flushed
        /// </summary>
        const int MAX_EVENTS = 10;

        /// <summary>
        /// The event queue
        /// </summary>
        BlockingCollection<IEvent> EventQueue { get; }  = new BlockingCollection<IEvent>(MAX_EVENTS);

        /// <summary>
        /// Number of seconds before adding to the event queue fails
        /// </summary>
        const int _eventQueueTimeout = 1;

        /// <summary>
        /// Time (milliseconds) between flushing the events to the server in the background.
        /// </summary>
        const int _backgroundFlushInterval = (60 * 1000);

        /// <summary>
        /// URL of the event receiver
        /// </summary>
        public string EVENT_API_BASE_URL { get; } = "https://events.floodgate.io";

        /// <summary>
        /// Boolean representing if the event queue is busy being processed
        /// </summary>
        private AtomicBoolean isEventQueueBusy = new AtomicBoolean();

        /// <summary>
        /// Set to true when the application is exiting to stop accepting any new events into the queue
        /// </summary>
        private AtomicBoolean isExiting = new AtomicBoolean();

        /// <summary>
        /// Set when a flush event occurs
        /// </summary>
        private AtomicBoolean isFlushing = new AtomicBoolean();

        Timer backgroundFlush;

        private List<IEvent> eventBuffer;

        ILogger Logger;

        EventsConfig _config;

        private readonly object bufferLock = new object();

        public EventProcessor(ILogger logger, EventsConfig config)
        {
            Logger = logger;

            _config = config;

            Initaise();
        }

        private void Initaise()
        {
            eventBuffer = new List<IEvent>();

            backgroundFlush = new Timer(BackgroundFlush, null, _backgroundFlushInterval, _backgroundFlushInterval);

            Task.Run(() => RunEventProcessLoop()).ConfigureAwait(false);
        }

        /// <summary>
        /// Try to add an event to the event queue. If the queue is full then force Flush
        /// </summary>
        /// <param name="e">The event to be added to the queue</param>
        public void AddToQueue(IEvent e)
        {
            if (eventBuffer.Count >= MAX_EVENTS)
            {
                Logger.Warning("Event queue full, force a Flush!");

                // Flush queue here then proceed adding the new event
                EventQueue.Add(new FlushQueueEvent("MAX_EVENTS"));
            }

            try
            {
                if (!isEventQueueBusy.Get() && !isExiting.Get()) // If not busy && not exiting then add to queue
                //if (!isEventQueueBusy.Get()) // If not busy && not exiting then add to queue
                {
                    // Add event to the queue
                    EventQueue.Add(e);
                }
            }
            catch(InvalidOperationException)
            {
                Logger.Error("EventQueue has been shutdown");
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Flushes any events in the event queue to the FloodGate servers
        /// It is called on either the backgroundFlush timer, the event processing thread or once the application is disposed
        /// </summary>
        public void Flush()
        {
            Logger.Info("Flushing Events");

            if (eventBuffer.Count == 0)
            {
                Logger.Info("No events to flush");
                return;
            }

            try
            {
                if (!isFlushing.GetAndSet(true))
                {
                    lock (bufferLock)
                    {
                        JArray json = JArray.FromObject(eventBuffer);

                        eventBuffer.Clear();
                        
                        Logger.Info("Event Buffer Cleared");

                        TransmitEvents(json.ToString());
                    }

                    isFlushing.Set(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                throw ex;
            } 
        }

        private void BackgroundFlush(object state)
        {
            Logger.Info("Start Background Flush");
            AddToQueue(new FlushQueueEvent("BackgroundFlush"));
        }

        /// <summary>
        /// Process any events in the event queue and send them to the FloodGate servers
        /// Process the queue every 1 second
        /// </summary>
        private void RunEventProcessLoop()
        {
            bool active = true;

            IEvent item = null;

            try
            {
                while (active)
                {
                    item = EventQueue.Take();
                    
                    switch (item)
                    {
                        case FlushQueueEvent f:
                            Logger.Info("Processing FlushQueueEvent");
                            
                            Flush();

                            break;
                        case ShutdownEvent s:
                            Logger.Info("Processing ShutdownEvent");
                            
                            Flush();
                            
                            isExiting.Set(true);
                            
                            active = false;

                            break;
                        case FlagEvaluationEvent fe:
                        case FlagNotFoundEvent fnf:
                            // Is there anything I need to do here relating to other events
                            // Add it to this threads buffer

                            lock (bufferLock)
                            {
                                eventBuffer.Add(item);
                            }

                            break;
                    }

                    Logger.Info($"eventBuffer.Count = {eventBuffer.Count}");
                }
            }
            catch(Exception ex)
            {
                Flush();

                isExiting.Set(true);

                Logger.Error($"An error occured in the event processor, trying forced flush : {ex.Message}");
            }
        }

        public Uri BuildEventsUrl()
        {
            if (!string.IsNullOrEmpty(_config.EventsUrl))
            {
                var url = $"{_config.EventsUrl}/api/v1/ingest";

                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                    return new Uri(url);
            }

            return new Uri($"{EVENT_API_BASE_URL}/api/v1/ingest");
        }

        /// <summary>
        /// Send the events to the server
        /// </summary>
        private async void TransmitEvents(string eventsPayload)
        {
            Logger.Info($"TransmitEvents()");

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Agent", "dotnet");
            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Version", ClientConfigBase.AssemblyVersion);
            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Message", "Event");

            var data = new StringContent(eventsPayload, Encoding.UTF8, "application/json");

            try
            {
                await httpClient.PostAsync(BuildEventsUrl(), data);
            }
            catch(Exception ex)
            {
                Logger.Error($"Failed to transmit events : {ex.Message}");
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public void ManualFlush()
        {
            Logger.Info("Manual Flush");
            EventQueue.Add(new FlushQueueEvent("MANUAL_FLUSH"));
        }

        public void Dispose()
        {
            // TODO: If there is already a Flush() happening then no need to perform another Flush()

            Logger.Info("Shutting Down");

            try
            {
                backgroundFlush.Dispose();

                AddToQueue(new ShutdownEvent());

                //isExiting.Set(true);

                //EventQueue.CompleteAdding();
            }
            finally
            {
                EventQueue.Dispose();

                GC.SuppressFinalize(this);
            }
        }

        
    }
}
