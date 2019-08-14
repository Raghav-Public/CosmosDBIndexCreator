FROM mcr.microsoft.com/dotnet/core/runtime:2.1

COPY CosmosDBIndexCreator/bin/Debug/netcoreapp2.1/publish/ CosmosDBIndexCreator/

ENTRYPOINT ["dotnet", "CosmosDBIndexCreator/CosmosDBIndexCreator.dll"]
CMD ["argument"]