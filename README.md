# RiskCaptureDemo

Setup eventstore as follows to run this example:

You must start up eventstore with the projections enabled:

EventStore.ClusterNode.exe --db ./db --log ./logs --run-projections=all

ensure all default projections (such as fromcategory etc) are enabled from the console as they are not by default:
http://127.0.0.1:2113/web/index.html#/projections (click 'enable all')

add a continuous emitting projection in eventstore called RiskCapture Indexer with the body:

fromCategory("riskcapture")
  .whenAny(function(s,e) {
    linkTo('riskcaptures',e);
  });
  
This will link events from all streams in the category of riskcapture to a stream called riskcaptures so that subscribers can get all events in this category in one subscription

add another continuous emitting projection in eventstore called RiskCapture Lookup with the body:

fromStream("riskcapture-riskcapturemap")
.when( 
{ 
    newRiskItemMapped : function(s,e) 
    {
        emit(
            "riskcaptureprojections-riskcapturemaplookup-" + e.body.ProductLine, 
            "lookup", 
            {
                "itemId" : e.body.RiskItemId,
                "sectionName" : e.body.SectionName,
                "itemName" : e.body.ItemName
            });
    }
});

This will create a projection from the risk capture map stream that will allow the lookup of items by id.

