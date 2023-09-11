
lances
skydragon's fury
scourge of the corrupter


Known Bugs:

- hold style modifications via the config menu will not update until weapon switch -> minor, wontfix
- melee effect experimental feature: phase blades and phase sabers have flickering lights

MP Error: when one player joins after another (assume because no hold style communicated) since no weapon is initially visible

[11:52:30.538] [Main Thread/WARN] [tML]: Silently Caught Exception: 
System.NullReferenceException: Object reference not set to an instance of an object.
   at Hook<System.Void Terraria.ModLoader.Engine.LoggingHooks::Hook_StackTrace_CaptureStackTrace(Terraria.ModLoader.Engine.LoggingHooks+orig_StackTrace_CaptureStackTrace,System.Diagnostics.StackTrace,System.Int32,System.Boolean,System.Exception)>(StackTrace , Int32 , Boolean , Exception )
   at SyncProxy<System.Void System.Diagnostics.StackTrace:CaptureStackTrace(System.Int32, System.Boolean, System.Exception)>(StackTrace , Int32 , Boolean , Exception )
   at System.Diagnostics.StackTrace..ctor(Boolean fNeedFileInfo)
   at Terraria.ModLoader.Logging.FirstChanceExceptionHandler(Object sender, FirstChanceExceptionEventArgs args) in tModLoader\Terraria\ModLoader\Logging.ExceptionHandling.cs:line 105
   at WeaponOutLite.Common.Players.WeaponOutPlayerRenderer.manageBodyFrame() in WeaponOutLite\Common\Players\WeaponOutPlayerRenderer.cs:line 227
   at Terraria.ModLoader.PlayerLoader.PostUpdate(Player player) in tModLoader\Terraria\ModLoader\PlayerLoader.cs:line 367
   at Terraria.Player.Update(Int32 i) in tModLoader\Terraria\Player.cs:line 23018
   at Terraria.Main.DoUpdateInWorld(Stopwatch sw) in tModLoader\Terraria\Main.cs:line 14872
   at Terraria.Main.DoUpdate(GameTime& gameTime) in tModLoader\Terraria\Main.cs:line 14508
   at Terraria.Main.Update(GameTime gameTime) in tModLoader\Terraria\Main.cs:line 14046
   at Microsoft.Xna.Framework.Game.Tick() in D:\a\tModLoader\tModLoader\FNA\src\Game.cs:line 546
   at Microsoft.Xna.Framework.Game.RunLoop() in D:\a\tModLoader\tModLoader\FNA\src\Game.cs:line 878
   at Microsoft.Xna.Framework.Game.Run() in D:\a\tModLoader\tModLoader\FNA\src\Game.cs:line 419
   at Terraria.Program.RunGame() in tModLoader\Terraria\Program.cs:line 271
   at Terraria.Program.LaunchGame_(Boolean isServer) in tModLoader\Terraria\Program.cs:line 245
   at Terraria.Program.LaunchGame(String[] args, Boolean monoArgs) in tModLoader\Terraria\Program.cs:line 220
   at System.Threading.Thread.StartCallback()
