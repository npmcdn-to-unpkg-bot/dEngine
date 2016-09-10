local RunService = game:GetService("RunService")

local OrbDictionary = {}
local OrbsContainer = Instance.new("Model", core)
OrbsContainer.Name = "Orbs"

-- class: Orb
local Orb = {}

function Orb.new (player, colour)
	local self = setmetatable({}, MyClass)

	local transparency = 0--0.7

	local model = Instance.new("Model", OrbsContainer)
	model.Name = player.Name

	local sphere = Instance.new("Part", model)
	sphere.Name = "Head"
	sphere.Shape = Enum.Shape.Sphere
	sphere.Size = Vector3.new(1.8, 1.8, 1.8)
	sphere.BrickColour = colour
	sphere.Transparency = transparency
	model.PrimaryPart = sphere

	local cylinder = Instance.new("Part", model)
	cylinder.Name = "Cylinder"
	cylinder.Shape = Enum.Shape.Cylinder
	cylinder.Size = Vector3.new(0.25, 2.5, 0.25)
	..Transparency = transparency
	cylinder.BrickColour = colour

	local cone = Instance.new("Part", model)
	cone.Name = "Cone"
	cone.Shape = Enum.Shape.Cone
	cone.Size = Vector3.new(0.356, 0.535, 0.356)
	cone.Transparency = transparency
	cone.BrickColour = colour
	
	self.Player = player
	self.Model = model
	self.Position = Vector3.new(0, 0, 0)
	self.Direction = Vector3.new(0.934286058, -0.00014666545, -0.356524438)

	RunService.Heartbeat:connect(function()
		self:Update()
	end)

	return self
end

function Orb:Update()
	local cf = CFrame.new(self.Position, self.Position + self.Direction)
	Model.Head.CFrame = cf
	Model.Sphere.CFrame = cf * CFrame.Angles(0, 0, math.pi/2)
end
------------------------

-- TODO: remove test
local orb = Orb.new(null, Colour.Red)
print("Orb created: ", orb)

