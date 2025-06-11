VAR has_key = false

{has_key:-> UNLOCKED|-> LOCKED}

==LOCKED==
It's locked...
Where did Mom and Dad leave the key again?
    -> END

==UNLOCKED==
The key fits perfectly into the keyhole
\*click\*
    -> END
