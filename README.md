# Jzt Oct 2023

## Description
An rpg mmo style game

## Sections
There are a few core components to the system
* Client - Game client
* Server - Game server used to manage and serve data for in game, such as connected players, resources, enemies locations, as well as managing some server-based features.
* Authentication - Auth Server used to authenticate users in AWS user pool and give / validate tokens
* Character - Character Server used to serve / store / update character data. Has a REST API as well as websocket communication for real time updates from Game Server.

## Design docs
https://docs.google.com/document/d/1lITH8mZTxfJIvKmDfllnNTu1q2WSlluFcifHb9xySP4/edit

## Communication

### Client Login

```mermaid
sequenceDiagram
	participant Client
	participant AuthServer
	participant GameServer
	Client->>AuthServer: /authenticateUser (Username & Password)
	loop Challenges
		AuthServer->>Client: Challenge & Login session token
		Client->>AuthServer: /challengeUser (Answer & Login sesion token)
	end
	AuthServer->>Client: IdToken, AccessToken, RefreshToken
	Client->>GameServer: Spawn Request (AccessToken)
	GameServer->>AuthServer: /verifyToken (AccessToken)
	alt token is valid
	AuthServer->>GameServer: http 200
	GameServer->>Client: Spawn Location
	else token is bad
	AuthServer->>GameServer: http 500
	end
```

### Character Rest API
```mermaid
sequenceDiagram
	participant Client
	participant CharacterAPI
	participant AuthServer

	note right of Client: Client may be different than game client
	note right of Client: Needs AccessToken Priorer

	Client->>CharacterAPI: (Private API) authentication header with AccessToken
	CharacterAPI->>AuthServer: /verifyToken (AccessToken)
	alt token is valid
	AuthServer->>CharacterAPI: http 200
	CharacterAPI->>Client: API Response
	else token is bad
	AuthServer->>CharacterAPI: http 500
	CharacterAPI->>Client: http 403
	end
```