param (
    [Parameter(Mandatory=$true)][int]$n = 1,
    [Parameter(Mandatory=$true)][string]$sku
)

Write-Host sku: $sku

$filePath = "C:\Users\azureuser\galaxy-service-bus\QueueReceiver\bin\Debug\net6.0\QueueReceiver.exe"
$queueName = "myqueue"
$connStrStd = "Endpoint=sb://vitasapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=F/XXX"
$connStrPrm = "Endpoint=sb://vitasapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=F/XXX"

for ($i=0;$i -lt $n; $i++) {

    if ($sku -eq "premium") {
        Start-Process -FilePath $filePath -ArgumentList $queueName,$connStrPrm  
    } else {
        Start-Process -FilePath $filePath -ArgumentList $queueName,$connStrStd
    }

}