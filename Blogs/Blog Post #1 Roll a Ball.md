# Blog Post #1 : Roll a Ball

### Introduction

As a start for the game development course (GMD) at VIA University College, we had to follow a short tutorial to toy with Unity's functionalities and components. This involved creating a ball game object, adding some components to it for physical movement, a script to move the ball using the Input System, and finally adding some User Interface to do various actions.

### Level 1

I went ahead and created two levels for this mini game. The first level, shown in the picture below, is an obstacle course. This involves certain walls and ramps, where the player will have to control the ball and collect some points. There are different points that the player can collect, which have different values based on the point's colour. The aim is to collect all the points and get to the end of the level. To avoid the obstacles, the player uses the WASD keys to control the ball's directional movement, and the SPACE key to jump. Double jump is also available for the jumping action. The player can choose to restart the level at any time, by pressing the Restart button found on the screen.

![BonusLevel](/pictures/Roll-a-Ball-Bonus-Level.png)

### Bonus Level

The second level, acting like a Bonus level, can be accessed by pressing the Bonus Level button found on screen. This teleports the ball to a new level. In this level, there are five different image placeholders at the top of the screen. Further, the level contains five walls, split into two colours: red and black. The logic behind this level is that when it loads, the game generates a five random item sequence with the colours red and black. The player needs to guess the sequence generated. This can be done by going through the coloured walls in the level. If the player chooses the right colour coming in the sequence, the colour will be displayed in the image at the top, then the player needs to guess the next one, and so on. Any mistake along the choices will restart the level. This is more of a gambiling level, where the player just luckily guesses the next colour in the sequence. The image below shows an example of this gambling logic.

![BonusLevel](/pictures/Roll-a-Ball-Bonus-Level.png)
