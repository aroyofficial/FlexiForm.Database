param (
    [string]$Type,
    [string]$Name,
    [string]$Author
)

try {
    $environment = if ($env:ASPNETCORE_ENVIRONMENT) {
        $env:ASPNETCORE_ENVIRONMENT
    } else {
        "Unknown"
    }

    Write-Host "Detected environment: $environment"

    if ($environment.ToLower() -eq "development") {
        Set-Location -Path "D:\FlexiForm.Database"
    }

    $typeToFolderMap = @{
        "schema"  = "Schemas"
        "proc"    = "StoredProcedures"
        "constraint" = "Constraints"
    }

    if (-not $Type) {
        $Type = Read-Host "Enter script type (schema, proc, constraint)"
    }

    $Type = $Type.Trim().ToLower()
    if (-not $typeToFolderMap.ContainsKey($Type)) {
        throw "❌ Invalid script type: '$Type'. Must be one of: $($typeToFolderMap.Keys -join ', ')"
    }

    if (-not $Name) {
        $Name = Read-Host "Enter master object name (e.g., tblUsers, usp_CreateUser)"
    }

    if (-not $Author) {
        $Author = Read-Host "Enter author name"
    }

    if ([string]::IsNullOrWhiteSpace($Author)) {
        throw "❌ Author name is required"
    }

    $utcNow = (Get-Date).ToUniversalTime()
    $uniqueNumber = $utcNow.ToString("yyyyMMddHHmmssfff")

    $nameWithoutExtension = [System.IO.Path]::GetFileNameWithoutExtension($Name)
    $sanitizedName = $nameWithoutExtension -replace '\s+', '_' -replace '[^a-zA-Z0-9_]', ''
    $sanitizedName = $sanitizedName.ToLower()

    if ([string]::IsNullOrWhiteSpace($sanitizedName)) {
        throw "❌ Script name is required"
    }

    $filename = "${uniqueNumber}_${Type}_$sanitizedName.sql"

    $basePath = Get-Location
    $scriptsFolder = Join-Path $basePath "FlexiForm.Database/Scripts"
    $typeSubFolder = $typeToFolderMap[$Type]

    $upFolder = Join-Path -Path $scriptsFolder -ChildPath (Join-Path $typeSubFolder "Up")
    $downFolder = Join-Path -Path $scriptsFolder -ChildPath (Join-Path $typeSubFolder "Down")

    foreach ($folder in @($upFolder, $downFolder)) {
        New-Item -ItemType Directory -Path $folder -Force -ErrorAction SilentlyContinue | Out-Null
    }

    $upFilePath = Join-Path $upFolder $filename
    $downFilePath = Join-Path $downFolder $filename

    $header = @"
-- Script Type    : $Type
-- Name           : $filename
-- Created At     : $($utcNow.ToString("yyyy-MM-dd HH:mm:ss")) UTC ($Author)
-- Script ID      : $uniqueNumber
"@

    $upContent = @"
$header
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    -- TODO: Add your SQL script here (Up)

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
"@

    $downContent = @"
$header
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    -- TODO: Add your SQL script here (Down)

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
"@

    $upContent | Out-File -FilePath $upFilePath -Encoding UTF8 -Force
    $downContent | Out-File -FilePath $downFilePath -Encoding UTF8 -Force

    Write-Output "✅ SQL scripts created:"
    Write-Output "📄 Up:   $upFilePath"
    Write-Output "📄 Down: $downFilePath"
}
catch {
    Write-Error "❌ Caught an error: $($_.Exception.Message)"
}