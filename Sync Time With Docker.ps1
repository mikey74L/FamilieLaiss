Write-Host "Synchronizing your docker time."

$principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if ($principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
	$name = (Get-VMIntegrationService -VMName DockerDesktopVM | Where-Object { $_.Id.StartsWith("Microsoft:") -and $_.Id.EndsWith("\2497F4DE-E9FA-4204-80E4-4B75C46419C0") }).Name # "Time Synchronization" for English Windows
	Get-VMIntegrationService -VMName DockerDesktopVM -Name $name | Disable-VMIntegrationService
	Get-VMIntegrationService -VMName DockerDesktopVM -Name $name | Enable-VMIntegrationService
	echo $name
	Write-Host "Time synced!"
	Read-Host -Prompt "Press Enter to exit."
} else {
	Write-Host "Elevated privileges are required."
	Start-Process -FilePath "powershell" -ArgumentList "$('-File ""')$(Get-Location)$('\')$($MyInvocation.MyCommand.Name)$('""')" -Verb runAs
}