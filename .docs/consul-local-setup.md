## Set up Consul

There's extensive documentation available on [Consul.io](https://www.consul.io) about setting up a stable Consul cluster, and it doesn't make sense to repeat that here. However, for your convenience, we include this guide so you can quickly get Orleans running with a standalone Consul agent.

1. Create a folder to install Consul into (for example _C:\Consul_).
1. Create a subfolder: _C:\Consul\Data_ (Consul doesn't create this directory if it doesn't exist).
1. [Download](https://www.consul.io/downloads.html) and unzip _Consul.exe_ into _C:\Consul_.
1. Open a command prompt at _C:\Consul_ and run the following command:

   ```powershell
   ./consul.exe agent -server -bootstrap -data-dir "C:\Consul\Data" -client='0.0.0.0'
   ```

   In the preceding command:

   - `agent`: Instructs Consul to run the agent process that hosts the services. Without this switch, the Consul process attempts to use RPC to configure a running agent.
   - `-server`: Defines the agent as a server and not a client (A Consul _client_ is an agent that hosts all the services and data, but doesn't have voting rights to decide, and can't become, the cluster leader.
   - `-bootstrap`: The first (and only the first!) node in a cluster must be bootstrapped so that it assumes the cluster leadership.
   - `-data-dir [path]`: Specifies the path where all Consul data is stored, including the cluster membership table.
   - `-client='0.0.0.0'`: Informs Consul which IP to open the service on.

   There are many other parameters, and the option to use a JSON configuration file. For a full listing of the options, see the Consul documentation.

1. Verify that Consul is running and ready to accept membership requests from Orleans by opening the services endpoint in your browser at `http://localhost:8500/v1/catalog/services`. When functioning correctly, the browser displays the following JSON:

    ```json
    {
        "consul": []
    }
    ```