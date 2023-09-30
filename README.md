# Valheim Player Manager
This project was built as an extension of a Valheim game server. 
This application utilizes web endpoints to recieve player join and disconnect information and decode it through the steam API to relay the retrieved player username to a designated discord channel webhook. 

All keys and configuation is stored in the Linux enviornment variables when deploying via docker.

## Docker Compose File
```yaml
version: '3.3'
services:
  valheimplayermanager:
    ports:
      - '9996:80'
    environment:
      - DISCORD_WEBHOOK=<HOOK_HERE>
      - STEAM_APIKEY=<APIKEY_HERE>
    restart: always
    logging:
      options:
        max-size: 1g
    image: 'hastingsn25/valheimplayermanager:latest'
```
