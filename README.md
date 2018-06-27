# UnicastToMulticast
A tool to multicast unicast data.

# Usage

This is console app.

Input 5 parameters and start transfer.


```
From address:
From port:
Multicast nic(169.254.80.80):
Multicast address:
Multicast port:
192.168.1.1:5000 -> 239.0.0.1:5000
```

# Data source
 ## Tcp Client
 
 - Connect to tcp server
 - When data received,transfer to multicast
 
# Multicast Server

 - Multicast to the specified NIC
 

# Caution

This is a testing tool.

There is almost no exception handling.
