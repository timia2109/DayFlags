# DayFlags
A simple service to flag days. This repo is currently work in progress

## Idea
This service helps to mark days. It can be used to collect events in the past or to announce events in the future.

## Definition
### Realm
A realm is a defined area for the other objects. Everything within a realm is unique.

### FlagType
FlagTypes that define the allowed DayFlags. A FlagType can occur in a FlagGroup. 

### FlagGroup
A FlagGroup is a grouping of multiple FlagTypes. It can be used for queries. There is a flag if only one FlagType of the group is allowed on a date.

### DayFlag
DayFlags are used to specify the occurrence of a FlagType on a calendar date.