using System.Net;
using ValheimPlayerManager.Web_Server.Endpoints;

namespace ValheimPlayerManager.Web_Server
{
    /// <summary>
    /// Web Server to handle HTTP requests
    /// </summary>
    public class WebServer : IDisposable
    {
        private HttpListener _listener;
        private Thread _connectionThread = null;
        private bool _running, _disposed;

        private HttpResourceLocator _resouceLocator = null;

        /// <summary>
        /// Create new Web Server object
        /// </summary>
        /// <param name="prefix">Prefix for </param>
        /// <exception cref="NotSupportedException"></exception>
        public WebServer(string prefix)
        {
            //Check if running on supported OS
            if (!HttpListener.IsSupported)
                //If not throw unsupported error
                throw new NotSupportedException("The Http Server cannot run on this computer operating system");

            //Create new HTTP listender
            _listener = new HttpListener();
            //Add listener prefix
            _listener.Prefixes.Add(prefix);
            //Create new instance of resource locator for handling request
            _resouceLocator = new HttpResourceLocator();
        }

        /// <summary>
        /// Add new endpoint to Web Server
        /// </summary>
        /// <param name="endpoint">Endpoint to be added</param>
        public void AddEndpoint(IWebEndpoint endpoint)
        {
            _resouceLocator.AddEndpoint(endpoint);
        }

        /// <summary>
        /// Start Web Server Listener
        /// </summary>
        public void Start()
        {
            //Check if server is not running
            if (!_listener.IsListening)
            {
                //Start server
                _listener.Start();
                _running = true;
                //Create thread for handling clients
                _connectionThread = new Thread(new ThreadStart(HandleConnectionThread));
                //Start connection thread
                _connectionThread.Start();
            }
        }

        /// <summary>
        /// Stop Web Server
        /// </summary>
        public void Stop()
        {
            //Check if server is running
            if (_listener.IsListening)
            {
                _running = false;
                //Stop listener
                _listener.Stop();
                //Wait for client manager to join main
                _connectionThread.Join();
            }
        }

        /// <summary>
        /// Handle connecting clients
        /// </summary>
        private void HandleConnectionThread()
        {
            try
            {
                //While the server is running
                while (_running)
                {
                    //Receive incoming connection
                    HttpListenerContext context = _listener.GetContext();

                    //Send to client manager
                    _resouceLocator.HandleContext(context);
                }
            }
            catch { }
        }

        /// <summary>
        /// Dispose of web server object
        /// </summary>
        public virtual void Dispose()
        {
            //Set dispose
            Dispose(true);
            //Supress garbage collection
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose server object
        /// </summary>
        /// <param name="disposing">bool indicating if disposing</param>
        private void Dispose(bool disposing)
        {
            //Check if already disposed
            if (_disposed)
                return;

            //If disposing
            if (disposing)
            {
                //Stop server if running
                if (_running)
                    Stop();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Locator to handle client requests
    /// </summary>
    class HttpResourceLocator
    {
        private Dictionary<string, IWebEndpoint> _httpEndpoints;

        /// <summary>
        /// Create new locator
        /// </summary>
        public HttpResourceLocator()
        {
            //Create new storage list
            _httpEndpoints = new Dictionary<string, IWebEndpoint>();
            //Add default invalid request
            AddEndpoint(new InvalidWebRequest());
        }

        /// <summary>
        /// Add endpoint to locator
        /// </summary>
        /// <param name="endpoint">Endpoint to add</param>
        public void AddEndpoint(IWebEndpoint endpoint)
        { 
            //Check if endpoint exists
            if(!_httpEndpoints.ContainsKey(endpoint.GetEndpointName()))
                //If doesn't exist, add to locator
                _httpEndpoints.Add(endpoint.GetEndpointName(), endpoint);
            else
                //If exists, update endpoint
                _httpEndpoints[endpoint.GetEndpointName()] = endpoint;
        }

        /// <summary>
        /// Handle request from client
        /// </summary>
        /// <param name="context">Request context</param>
        public void HandleContext(HttpListenerContext context)
        {
            //Get request information
            HttpListenerRequest request = context.Request;
            //Get resource being requested
            string requestHeaderName = request.Url.AbsolutePath;

            IWebEndpoint endpoint;
            //Check if endpoint exists
            if (_httpEndpoints.ContainsKey(requestHeaderName))
                //If exists, set endpoint to be executed
                endpoint = _httpEndpoints[requestHeaderName];
            else
                //If not, set to invalid request
                endpoint = _httpEndpoints[InvalidWebRequest.NAME];

            //Execute request
            InvokeHandler(endpoint, context);
        }

        /// <summary>
        /// Execute endpoint request
        /// </summary>
        /// <param name="endpoint">Endpoint to be executed</param>
        /// <param name="context">Context for request</param>
        private void InvokeHandler(IWebEndpoint endpoint, HttpListenerContext context)
        {
            //Create new command object
            HandleEndpointCommand command = new HandleEndpointCommand(endpoint, context);
            //Create thread to handle request
            Thread requestThread = new Thread(command.Execute);
            //Execute request
            requestThread.Start();
        }

        /// <summary>
        /// Handle Endpoint Execution
        /// </summary>
        class HandleEndpointCommand
        {
            private IWebEndpoint _endpoint;
            private HttpListenerContext _context;

            /// <summary>
            /// Create new command object
            /// </summary>
            /// <param name="endpoint">Endpoint to be executed</param>
            /// <param name="context">Request context</param>
            public HandleEndpointCommand(IWebEndpoint endpoint, HttpListenerContext context)
            {
                _endpoint = endpoint;
                _context = context;
            }

            /// <summary>
            /// Execute request
            /// </summary>
            public void Execute()
            {
                _endpoint.ProcessRequest(_context);
            }
        }
    }
}
