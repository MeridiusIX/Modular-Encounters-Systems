#TextTemplate.md

TextTemplate files are custom XML files that allow you to define text that can be used in various applications. At the time of this page being written, these files can only currently be used for changing Block CustomData.

# How to Create and Use These Files

These files are not like `.sbc` files, they can not be placed in any directory in your mod. The files also must end in `.xml` extensions. Below is the folder/directory you must place these files in:

`YourModFolder\Data\TextTemplates\`

A full example would be:

`YourModFolder\Data\TextTemplates\SomeTextTemplateFile.xml`

A single file should contain a single TextTemplate. You cannot store multiple TextTemplates in a single file. When using your TextTemplate, you'll reference it by using its file name. Example:

`[SomeTagRequiringTextTemplate:SomeTextTemplateFile.xml]`

# File Setup

Depending on what you're using the file for, you'll use one of the following data structures below:

**Custom Data**  

```
<?xml version="1.0" encoding="utf-16"?>
<TextTemplate xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <CustomData>Some Sample Custom Data</CustomData>
</TextTemplate>
```

**Datapad**

```
<?xml version="1.0" encoding="utf-16"?>
<TextTemplate xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DataPadEntries>
    <DataPadEntry>
      <DataPadTitle>Some Title</DataPadTitle>
      <DataPadBody>Lorem Ipsum Yaddy Yadda</DataPadBody>
    </DataPadEntry>
  </DataPadEntries>
</TextTemplate>
```

**LCD Contents**

```
<?xml version="1.0" encoding="utf-16"?>
<TextTemplate xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <LcdEntries>
    <LcdEntry>
      <TextSurfaceIndex>0</TextSurfaceIndex>
      <ApplyLcdText>false</ApplyLcdText>
      <LcdText>Some Text For LCD</LcdText>
      <ApplyLcdImage>false</ApplyLcdImage>
      <LcdImages>SomeLcdImageSubtypeId</LcdImages>
      <LcdImages>AnotherLcdImageSubtypeId</LcdImages>
      <LcdImages>AndAnotherLcdImageSubtypeId</LcdImages>
      <LcdImageChangeDelay>3</LcdImageChangeDelay>
    </LcdEntry>
  </LcdEntries>
</TextTemplate>
```