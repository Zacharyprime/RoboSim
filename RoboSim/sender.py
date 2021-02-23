#!/usr/bin/env python
   2 
   3 import socket
   4 
   5 
   6 TCP_IP = '127.0.0.1'
   7 TCP_PORT = 5005
   8 BUFFER_SIZE = 1024
   9 MESSAGE = "Hello, World!"
  10 
  11 s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
  12 s.connect((TCP_IP, TCP_PORT))
  13 s.send(MESSAGE)
  14 data = s.recv(BUFFER_SIZE)
s.close()

print "received data:", data
