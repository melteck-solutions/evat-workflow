param ($pool_name, $site_name, $packagepath)

Reset-IISServerManager  -Confirm: $false 

#check if the app pool exists
if  ((Get-IISAppPool).name -eq $pool_name )
{
    echo "----------------------------"
    echo "|     Pool exists          |"
    echo "----------------------------"
}
else  {
    New-WebAppPool -Name $pool_name -Force
}

Reset-IISServerManager -Confirm:$false

#check if the site exists
if ((Get-IISSite).name -eq $site_name)
{
    echo "----------------------------"
    echo "|     Site exists          |"
    echo "----------------------------"
}
else {
    New-Website -Name $site_name -ApplicationPool $pool_name -Force -PhysicalPath $packagepath -Port 8083
}