param (
    [string[]]$Arguments
)

function Show-Help {
    Write-Host @"

📘 Run-Migration - Command Line Help

Available Flags:
  --r, --maxretry <int>        : Maximum retry count (Allowed: 1 to 5)
  --t, --timeout <int>         : Task timeout in seconds (Allowed: 1 to 100)
  --type <MigrationType>       : Type of migration (Enum: e.g., Up, Down, Both.)
  --incremental                : Sets migration mode to Incremental
  --full                       : Sets migration mode to Full
  --tgt, --targets <Targets>   : Execution targets (Enum: None, Proc, Schema, Constraint, All or combinations like Proc,Schema)
  --help                       : Displays this help message

Usage Example:
  Run-Migration --r 3 --t 5 --m Strict --type Both --tgt Proc,Schema

"@
}

try {
    if ($Arguments -contains "--help") {
        Show-Help
        return
    }

    $environment = if ($env:ASPNETCORE_ENVIRONMENT) {
        $env:ASPNETCORE_ENVIRONMENT
    } else {
        "Unknown"
    }

    if ($environment.ToLower() -eq "development") {
        Remove-Item -Path "D:\FlexiForm.Database\FlexiForm.Database\bin" -Recurse -Force -ErrorAction SilentlyContinue
        Set-Location -Path "D:\FlexiForm.Database"
        dotnet clean
        dotnet build --configuration Debug
        $outputDir = "D:\FlexiForm.Database\FlexiForm.Database\bin\Debug\net8.0"
        Set-Location -Path $outputDir
        $exePath = Join-Path $outputDir "FlexiForm.Database.exe"
        & $exePath @Arguments
    }
    else {
        Write-Host "⚠️ Environment '$environment' is not configured for execution."
    }
}
catch {
    Write-Error "❌ Caught an error: $($_.Exception.Message)"
}
