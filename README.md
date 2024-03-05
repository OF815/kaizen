### To change administrator password

    net user administrator in_the_future

### Proxy PAC URL

* Get current value

    Get-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL

* Set value
    Set-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL -Value 'http://192.168.1.2/proxy.pac'

* Remove
    Remove-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings' -Name AutoConfigURL

    
