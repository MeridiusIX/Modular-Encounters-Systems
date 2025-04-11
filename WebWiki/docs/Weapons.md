#Weapons.md

The Weapon System profile in RivalAI is used to manage how/when weapons should be activated when engaging targets. You can add a Weapon System profile to your Behavior profile by using the `[WeaponSystem:ProfileSubtypeIdHere]` tag. Below is an example of how these profiles are setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
<EntityComponents>

<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>Example-ProfileSubtypeIdHere</SubtypeId>
      </Id>
      <Description>

        [RivalAI Weapons]

        [UseStaticGuns:true]
        [UseTurrets:true]

        [MaxStaticWeaponRange:5000]
        [WeaponMaxAngleFromTarget:6]
        [WeaponMaxBaseDistanceTarget:20]

        [UseBarrageFire:false]
        [MaxFireRateForBarrageWeapons:200]

        [UseAmmoReplenish:true]
        [AmmoReplenishClipAmount:15]
        [MaxAmmoReplenishments:10]

      </Description>
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

The following tags can be used in your Weapon System Profiles:


<!--UseStaticGuns-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseStaticGuns|
|:----|:----|
|Tag Format:|`[UseStaticGuns:Value]`|
|Description:|This tag specifies if an NPC grid should fire Static Weapons when engaging targets. Only works on behaviors that can rotate towards their target.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UseTurrets-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseTurrets|
|:----|:----|
|Tag Format:|`[UseTurrets:Value]`|
|Description:|This tag specifies if an NPC grid should manage turret weapons using extra features in RivalAI. This tag does NOT diable turrets (use MES to do that)|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MaxStaticWeaponRange-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxStaticWeaponRange|
|:----|:----|
|Tag Format:|`[MaxStaticWeaponRange:Value]`|
|Description:|Specifies the maximum distance from a target before an NPC can fire Static Weapons.|
|Allowed Values:|Any number higher than `0`<br />`-1 is No Limit`|
|Multiple Tag Allowed:|No|

<!--WeaponMaxAngleFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponMaxAngleFromTarget|
|:----|:----|
|Tag Format:|`[WeaponMaxAngleFromTarget:Value]`|
|Description:|Specifies the maximum angle from a target before an NPC can fire Static Weapons.|
|Allowed Values:|Any number higher than `0`|
|Multiple Tag Allowed:|No|

<!--WeaponMaxBaseDistanceTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponMaxBaseDistanceTarget|
|:----|:----|
|Tag Format:|`[WeaponMaxBaseDistanceTarget:Value]`|
|Description:|Specifies the maximum distance the end of the current shot trajectory can be from the intended target. Example: Imagine 2 lines from the gun, one going to the target, and another going the same distance, but in the forward direction of the gun. The distance between the 2 end points is what is checked and regulated by this tag.|
|Allowed Values:|Any number higher than `0`|
|Multiple Tag Allowed:|No|

<!--UseBarrageFire-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseBarrageFire|
|:----|:----|
|Tag Format:|`[UseBarrageFire:Value]`|
|Description:|This tag specifies static weapons should be fired sequentially, instead of all at once.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MaxFireRateForBarrageWeapons-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxFireRateForBarrageWeapons|
|:----|:----|
|Tag Format:|`[MaxFireRateForBarrageWeapons:Value]`|
|Description:|Specifies the max fire rate of a weapon to be considered usable for barrage fire. This is used to allow weapons that have a higher fire rate (eg Gatling Guns) to fire constantly instead of in a barrage sequence.|
|Allowed Values:|Any number higher than `0`|
|Multiple Tag Allowed:|No|

<!--UseAmmoReplenish-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseAmmoReplenish|
|:----|:----|
|Tag Format:|`[UseAmmoReplenish:Value]`|
|Description:|This tag specifies if weapons should automatically be kept loaded (ie Infinite Ammo).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AmmoReplenishClipAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AmmoReplenishClipAmount|
|:----|:----|
|Tag Format:|`[AmmoReplenishClipAmount:Value]`|
|Description:|This tag specifies how many Ammo Magazine Clips should be added to a weapon block when Ammo Replenish happens.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MaxAmmoReplenishments-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAmmoReplenishments|
|:----|:----|
|Tag Format:|`[MaxAmmoReplenishments:Value]`|
|Description:|This tag specifies how many times Ammo Replenishment will run on a weapon block.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|
