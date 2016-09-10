local username, userId = unpack(args)

local RunService = game:GetService("RunService")
local Players = game:GetService("Players")

local inStudio = RunService:IsLevelEditor()
RunService:Run()

local player = Players:CreateLocalPlayer(userId)
if inStudio then
	player.Name = username
end


