# Ensure the script is run as Administrator
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Output "Please run this script as Administrator."
    exit
}

# Define variables
$CONSUL_PATH = "C:\consul"
$CONSUL_CONFIG_PATH = "$CONSUL_PATH\config"
$CONSUL_LOG_PATH = "$CONSUL_PATH\logs\consul.log"
$CONSUL_DATA_PATH = "$CONSUL_PATH\data"
$NODE_NAME = "MyConsulNode"  # Adjust this as needed for the node's name
$consul_download_url = "https://releases.hashicorp.com/consul/1.20.0/consul_1.20.0_windows_386.zip" # Updated to Consul 1.20.0 for Windows 386 architecture
$bindIP = "127.0.0.1" 

# Create necessary directories
New-Item -Path $CONSUL_PATH -ItemType Directory -Force
New-Item -Path $CONSUL_CONFIG_PATH -ItemType Directory -Force
New-Item -Path $CONSUL_LOG_PATH -ItemType File -Force
New-Item -Path $CONSUL_DATA_PATH -ItemType Directory -Force

# Download and unzip Consul
cd $CONSUL_PATH
Invoke-WebRequest -Uri $consul_download_url -OutFile "$CONSUL_PATH\consul.zip"
Expand-Archive -Path "$CONSUL_PATH\consul.zip" -DestinationPath $CONSUL_PATH -Force
Remove-Item "$CONSUL_PATH\consul.zip"

# Set up Consul as a Windows Service
$consulServiceParams = @(
    "agent",
    "-server",
    "-bootstrap",
    "-node $NODE_NAME",
    "-config-dir=$CONSUL_CONFIG_PATH",
    "-log-file=$CONSUL_LOG_PATH",
    "-data-dir=$CONSUL_DATA_PATH",
    "-bind=$bindIP",
    "-advertise=$bindIP",
    "-ui"
)
$consulService = @{
    Name = "consul"
    BinaryPathName = "$CONSUL_PATH\consul.exe " + $consulServiceParams -join ' '
    DisplayName = "Consul Service"
    StartupType = "Automatic"
    Description = "Consul Service for Service Discovery and Configuration Management"
}

# Check if the service already exists, if so, stop and delete it using sc.exe
if (Get-Service -Name "consul" -ErrorAction SilentlyContinue) {
    Stop-Service "consul" -Force -ErrorAction SilentlyContinue
    Start-Process -FilePath "sc.exe" -ArgumentList "delete", "consul" -NoNewWindow -Wait
    Start-Sleep -Seconds 3  # Wait for the service to be fully removed
}

# Install the Consul service
try {
    New-Service @consulService
    Write-Output "Consul service created successfully."
} catch {
    Write-Output "Failed to create the Consul service: $_"
    exit
}

# Start the Consul service
try {
    Start-Service -Name "consul"
    Write-Output "Consul service started successfully."
} catch {
    Write-Output "Failed to start the Consul service: $_"
}

# Confirm service is running
try {
    if ((Get-Service -Name "consul").Status -eq 'Running') {
        Write-Output "Consul service is running."
    } else {
        Write-Output "Consul service is not running as expected."
    }
} catch {
    Write-Output "Error retrieving Consul service status: $_"
}
