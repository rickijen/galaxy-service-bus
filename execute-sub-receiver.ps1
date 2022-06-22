param (
    [Parameter(Mandatory=$true)][int]$n = 1,
    [Parameter(Mandatory=$true)][string]$sku
)

Write-Host sku: $sku

$filePath = "C:\Users\azureuser\galaxy-service-bus\SubscriptionReceiver\bin\Debug\net6.0\SubscriptionReceiver.exe"
$topicName = "mytopic"
$subName = "mysubscription"
$connStrStd = "Endpoint=sb://vitasapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=F/XXX"
$connStrPrm = "Endpoint=sb://vitasapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=F/XXX"

for ($i=0;$i -lt $n; $i++) {

    if ($sku -eq "premium") {
        Start-Process -FilePath $filePath -ArgumentList $topicName,$subName,$connStrPrm
    } else {
        Start-Process -FilePath $filePath -ArgumentList $topicName,$subName,$connStrStd
    }

}