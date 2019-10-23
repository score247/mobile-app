"mpctools\win-x64\mpc.exe" -i "..\src\Shared\Core\LiveScore.Core.csproj" -o "..\src\Shared\Core\CoreModelResolver.cs" -r "CoreModelResolver"

type temp.cs >> "..\src\Shared\Core\temp.cs"
type "..\src\Shared\Core\CoreModelResolver.cs" >> "..\src\Shared\Core\temp.cs"
del "..\src\Shared\Core\CoreModelResolver.cs"
type "..\src\Shared\Core\temp.cs" >> "..\src\Shared\Core\CoreModelResolver.cs"
del "..\src\Shared\Core\temp.cs"

"mpctools\win-x64\mpc.exe" -i "..\src\Shared\Sports\Soccer\Soccer.csproj" -o "..\src\Shared\Sports\Soccer\SoccerModelResolver.cs" -r "SoccerModelResolver"

type temp.cs >> "..\src\Shared\Sports\Soccer\temp.cs"
type "..\src\Shared\Sports\Soccer\SoccerModelResolver.cs" >> "..\src\Shared\Sports\Soccer\temp.cs"
del "..\src\Shared\Sports\Soccer\SoccerModelResolver.cs"
type "..\src\Shared\Sports\Soccer\temp.cs" >> "..\src\Shared\Sports\Soccer\SoccerModelResolver.cs"
del "..\src\Shared\Sports\Soccer\temp.cs"