# Unity Shooter Game

## Gameplay description

Castle Siege Shootout is a shooter game where you play as a player inside a castle. Your goal is to defend the castle against five enemies. You have an NPC ally who helps you by shooting at the enemies. Engage in intense gunfights, strategically position yourself, and eliminate the advancing foes. Survive the waves of enemies and protect the castle to win the game.
<br>
There are 3 difficulties: easy, medium and hard, which determine the damage of bullets.

## Simulation functionalities

- Clouds Shader: We implementing a Java code that generates an image with Perlin Noise, used for cloud material. With this, we made some particles in the sky. Aside that, we made a cloud shader that moves slowly with URP. We controlled paramters of shader like color, scale, speed etc.
- AI for enemies: The enemies have 2 stages: Patrol & Movement. We used NavMeshAgent which is responsible for moving the enemies through scene. In the patrol phase, we randomly choose points within the range of enemy and move to it. The movement uses raycasting and calculatePath method to follow the player.
