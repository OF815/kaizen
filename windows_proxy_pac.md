### To change administrator password

    net user administrator in_the_future

### Proxy PAC URL

Get current value

    Get-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL

Set value
  
    Set-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL -Value 'http://192.168.1.2/proxy.pac'

Proxy PAC URL
  
    Remove-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL
    
    icacls public

    icacls become_public /grant "ANYDOMAIN\DHCP Administrators":(CI)(OI)(M)

    icacls become_public

    icacls become_public /remove "ANYDOMAIN\DHCP Administrators"

Firewall

    Set-NetFirewallRule -DisplayGroup "ファイルとプリンターの共有" -Enabled True -Profile Private,Domain

LGO.exe

https://learn.microsoft.com/en-us/archive/blogs/secguide/lgpo-exe-local-group-policy-object-utility-v1-0

https://www.microsoft.com/en-us/download/details.aspx?id=55319
