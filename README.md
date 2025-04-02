# Flee Exhausion

Code mod for [Vintage Story](https://www.vintagestory.at). Entities run at slower speeds when they are hurt.

## Configuration
The server config is in the ModConfig `fleeexhaustion-server.json`. `ExhaustStrength` is a float which changes how strong the effect is. Default is 1 (100%).

## Mod Integration
This mod adds two new configuration options to the `fleeentity` AI task:

| Key          | Type    | Description                                          | Default value          |
| ------------ | ------- | ---------------------------------------------------- | ---------------------- |
| minmovespeed | Float   | The lowest speed possible for this entity to run at. | movespeed divided by 2 |
| exhausts     | Boolean | Whether this entity is affected by this mod.         | true                   |

Consider the following JSON patch:

```json
{
  "file": "game:entities/land/gazelle",
  "op": "add",
  "path": "/server/behaviors/9/aitasks/0/exhausts",
  "value": false
}
```

This example patch would cause gazelles to not be slowed, regardless of their health status.

## Debug info

If entity debug info is enabled on both the server and the client:

```
/entity debug on
.clientconfig showentitydebuginfo True
```

the following debug attributes may be visible if the entity is exhaustible:

| Key             | Description                                                                                                                                                |
| --------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| movespeed       | The speed at which the entity currently flees at.                                                                                                          |
| minspeed        | The minimum move speed configured for this entity.                                                                                                         |
| exhauststrength | What the move speed ends up being multiplied by when accounting for the `exhaustStrength` world config. Equivalent to the reciprocal of this config value. |
