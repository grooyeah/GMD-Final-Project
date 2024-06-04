# Blog Post #5 : Dev Update 3

## Introduction

This blog post coveres the last details of the final project development for the GMD Course. These include adding audio clips to the game, adding the UI and final touches for completing the project.

## Audio Clips

Several audio clips have been added to the project to be triggered on certain actions happening inside the game. Their setup can be found in the [SoundManager](../gmd-final-project/Assets/Scripts/GameManagers/SoundManager.cs) class inside the project. Using the [ISoundManager](../gmd-final-project/Assets/Scripts/Interfaces/ISoundManager.cs) interface, these sound are played throughout the gameplay, based on the certain action happening. I chose some funny sounds to put in the game to fit the look of the characters, while also adding manually some reverb inside FL Studio to them, to create a 'dungeon like' sound design.

## User Interface and other additions

I have added a start screen and an end screen to the game. These are used to start the game, restart it on finishing the game, and exiting the game. Additionally, I have also implemented a health bar system for both the player and the enemies, to visually track the remaining health of these entities throughout the fights and overall gameplay of the game.

In this update, I have also added the scoring system for the game. This score is not stored anywhere on completion of the levels, but is nice to keep track of how many points you got slaying enemies and destroying objects throughout the levels.

This update also comes with a screen when the player dies in the game. This provides the player with two options: an option to restart the current level and an option to gamble for a second chance. The gamble component is a coin flip game, where if you choose the right side, you get to continue playing the game. The win of the coin flip comes with 50 health points added to the player, while also pushing the enemies around the player, making room for recovering from the previous scenario that got the player dead.

The light for the game was limited to a circle area around the player, making it harder for the players to navigate around. This was implemented to enchance the dungeon feel of the levels, having limited vision around the dungeon rooms.

This development focused more on polishing the game before the its final version. Multiple tests were ran by going and completing all the levels in the game, which ensured the game's correct and intended functionality.
