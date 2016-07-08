Function Summary
1.From Unity inspector, editing specs of objects (skills, enemies, weapons, etc.), and save/load between xml file
2.add/delete array items; choose derived class from inspector
3.Support polymorphism inspector, displaying indices in array, names and dynamic types of elements
4.Alphabetical order of elements, sorted upon saving to xml

Known Issues
1.SpecStore serialization is unoptimized

Setup
1.Make base classes (Skill, Weapon, Armor, etc) derived from SpecObject
2.Make spec classes (AOESkill, ChargeSkill) derived from base classes (Skill) mentioned in step 1
3.Add XmlInclude attribute with spec classes (e.g. [XmlInclude(typeof(AOESkill))]) right before declaration of SpecObject class
4.Add XmlElement attribute with spec classes (e.g. [XmlElement(typeof(AOESkill))]) right before declaration of storeArray

Usage:
1.Drag SpecStore script
2.Select all related types in inspector (So add attributes here), can specify filter of single word
3.Specify path of xml file relative to Asset folder to filePath field (i.e. /Spec/Skill/SkillStore.xml)
4.Select the type of new element to add 
5.Click check boxes then press "remove" to delete elements
6.Hit "apply" after modifying the list

