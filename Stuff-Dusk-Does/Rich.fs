module Stuff

open DiscordRPC
open System
open System.Diagnostics
open System.Threading

let processes () = 
  let mutable result = []

  let ps = 
    Process.GetProcesses()
    |> Array.toSeq
    |> Seq.map (fun x -> (x.ProcessName, x.Threads))
    
  for (title, p) in ps do
    for proc in p do
      if proc.ThreadState = Diagnostics.ThreadState.Running then 
        result <- title :: result
  
  result

let animate (c: DiscordRpcClient) =
  let mutable count = 0
  let ran = Random()
  while true do

    let r = ran.NextDouble() * 4000.0
    count <- count + 1

    RichPresence (
      Details = "*boop*",
      State = sprintf "Boop number %i" count,
      Assets = Assets(
        LargeImageKey = "boop1",
        LargeImageText = "hehe"
      )
    ) |> c.SetPresence
    Thread.Sleep 1000

    RichPresence (
      Details = "...",
      State = sprintf "Boop number %i" count,
      Assets = Assets(
        LargeImageKey = "boop2",
        LargeImageText = "hehe"
      )
    ) |> c.SetPresence
    Thread.Sleep (int r)

let run () = 
  let client = new DiscordRpcClient("796667055990374400")
  client.OnReady.Add (fun s -> printfn "Collegato a %O%O" s.User.Username s.User.ID)
  client.Initialize() |> ignore
  animate client