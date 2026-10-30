Bun Voyage Changes
Jonathan Hoang
CS 583 Individual 2D Project

These are all the changes I made from my one pager and why I made them.


GAMEPLAY

1. Bottom bun starts on the plate
I made it so every burger already starts with a bottom bun on the plate instead of having it fall. When I was testing, waiting around for a bottom bun before I could do anything felt really slow, and every order needs one anyway.

2. Burgers don't get ruined anymore
Originally catching the wrong ingredient would ruin the burger. I changed it so you can stack whatever you catch and the burger only gets checked when you deliver it. If it matches the ticket you get paid. If it doesn't, the customer is unhappy and you lose a life. The trash is still there if you mess up and want to start over.

3. Top bun finishes the burger
Once the top bun lands the burger is done and anything else just falls through it. In my one pager anything landing on a finished burger would ruin it, but since burgers can't be ruined anymore I made finished burgers protected instead.

4. Three lives (stars)
In my one pager the run ended as soon as one customer ran out of patience. I thought that was too harsh, so now you get three lives shown as stars in the top right. You lose one if you deliver the wrong burger or if a customer runs out of patience, and the run ends when all three are gone.

5. Score is profit now
Instead of counting burgers, each correct burger earns money. A burger is worth $5 plus $2 for every filling, so bigger orders pay more. The high score shows up as Top Profit Score and it saves with PlayerPrefs so it stays between sessions.

6. Every order has a patty
I made sure every order has at least one patty because a burger without a patty didn't really feel like a burger. If the random fillings don't have one, one of them gets swapped for a patty.

7. Two to six fillings
My one pager said one to six fillings but one filling orders felt too short, so I changed the minimum to two.

8. Nothing spawns over the trash or the counter
Ingredients don't fall above the trash can or the delivery counter. This keeps those areas clear and makes going to them more fair.

9. Weighted spawning
This was one of the challenges I listed in my one pager. Ingredients that the order still needs fall more often, and the next one you need is the most likely. You still get ingredients you don't need, so you still have to be careful.

10. Sometimes two ingredients fall at once
Every so often two ingredients drop at the same time, spaced apart. I added this to make things feel a little busier.

11. Patience depends on the order
This was another challenge from my one pager. Customers wait 10 seconds plus 4 more seconds for each filling. So a two filling order gets 18 seconds and a six filling order gets 34 seconds.


CUSTOMERS

12. Four different customers
A random customer shows up behind the counter for each order, and you won't get the same one twice in a row. Each one has a happy, impatient, and angry face that changes as their patience meter goes down. The meter above their head also goes from green to yellow to red.


MENUS

13. How to Play
I added a How to Play button on the title screen that opens a popup with the controls and rules. You can close it with Enter or Escape.

14. Shop Closed screen
The results show up in their own scene called Shop Closed instead of a panel in the game. It shows your profit, your Top Profit Score, and a New Top Profit message if you beat your record.

15. Only Play Again
The results screen just has Play Again (or press Enter). I took out the option to go back to the title because I wanted it simple.


STUFF I CUT

16. Nasty items
The boot and rotten fish were supposed to ruin burgers. Since burgers can't be ruined anymore, they would just make the order wrong, which normal ingredients already do. So I cut them.

17. Countdown
The game starts right away when you press Start instead of doing a three second countdown.

18. Music speeding up
The music stays the same speed instead of speeding up when the meter is almost full.

19. Particle effects
I didn't end up adding the sparkle for delivering or the poof for trashing.

20. Some sounds
The game has background music, a bell when a new customer shows up, a grunt when a customer gets impatient, and a trash sound. I didn't add the catch plop, the splat, the cash register ding, or the angry huff.


CREDITS

Art: I ai generated the art with claude.ai

Audio: pixabay.com

