param (
    [Parameter(Mandatory=$true)][int]$n = 1,
    [Parameter(Mandatory=$true)][string]$sku
)

Write-Host sku: $sku

$filePath = "C:\Users\rijen\source\repos\galaxy-service-bus\SubscriptionReceiver\bin\Debug\netcoreapp3.0\SubscriptionReceiver.exe"
$topicName = "topic-00"
$subName = "sub-00"
$connStrStd = "Endpoint=sb://galaxy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gbyeXkuFjLcmC2d4YyQZQ+SbWMje0af88qxHBdMGUks="
$connStrPrm = "Endpoint=sb://galaxy-premium.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YfdWT2XPCw0RG+rt8BBvPUJxz6fSASYnv7krWc4UoME="

for ($i=0;$i -lt $n; $i++) {

    if ($sku -eq "premium") {
        Start-Process -FilePath $filePath -ArgumentList $topicName,$subName,$connStrPrm
    } else {
        Start-Process -FilePath $filePath -ArgumentList $topicName,$subName,$connStrStd
    }

}