param (
    [string[]]$Arguments
)

try {
    $environment = if ($env:ASPNETCORE_ENVIRONMENT) {
        $env:ASPNETCORE_ENVIRONMENT
    } else {
        "Unknown"
    }

    if ($environment.ToLower() -eq "development") {
        $baseDir = "D:\FlexiForm.Database\FlexiForm.Database\bin\Debug\net8.0"
        Set-Location -Path $baseDir
        $exePath = Join-Path $baseDir "FlexiForm.Database.exe"
        & $exePath @Arguments
    }
}
catch {
    Write-Error "❌ Caught an error: $($_.Exception.Message)"
}
