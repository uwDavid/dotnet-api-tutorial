app:
	dotnet run --project WebApp

api:
	dotnet run --project API

sapp:
	dotnet run --project WebApp --launch-profile https

sapi:
	dotnet run --project API --launch-profile https
