Push-Location "$PSScriptRoot"

terraform init -reconfigure -backend-config "./stages/debug.backend.tfvars"

$planFile = "./stages/debug.plan"

terraform plan -var-file="./stages/debug.tfvars" -out="$planFile"

$deploy = Read-Host -Prompt "Deploy? (y/n)"

if ($deploy -eq "y") {
    terraform apply -var-file="$PSScriptRoot/stages/debug.tfvars" $planFile
}

if (Test-Path $planFile) {
    Remove-Item $planFile
}

Pop-Location