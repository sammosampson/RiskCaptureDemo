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

Now to process some risk, using something like "postman" POST the following to http://localhost/RiskCapture/risk with a Content-Type application/xml header set:
(Change the Sequenceid GUID on subsequent posts and try adding new section items in)

```<ISMessage Direction="Request" Version="2.0" Function="PutTransRequest">
	<ISHeader>
		<SequenceID>6776193E-E1BD-43AC-B620-3DFEAF9B92B2</SequenceID>
	</ISHeader>
	<ISBody xmlns:qs="urn:quote-sys-private-data">
		<BusinessTransaction ProductLineCode="PM"/>
		<PolMessage>
			<PolData Type="Input">
				<ProposerPolicyholder>
					<ProposerPolicyholder_Prn Val="1"/>
					<ProposerPolicyholder_Type Val="P"/>
					<ProposerPolicyholder_ForenameInitial1 Val="Audrey"/>
					<ProposerPolicyholder_Surname Val="Bouvier Des Flandres"/>
					<ProposerPolicyholder_DateOfBirth Val="01/01/1970"/>
					<ProposerPolicyholder_AddressLine1 Val="43 Friskerton Lane"/>
					<ProposerPolicyholder_AddressLine2 Val="Test Town"/>
					<ProposerPolicyholder_AddressLine3 Val="Blue Square"/>
					<ProposerPolicyholder_AddressLine4 Val="Purpleton"/>
					<ProposerPolicyholder_AddressLine5 Val="AL6 0BH"/>
					<ProposerPolicyholder_AddressLine6 Val=""/>
					<ProposerPolicyholder_TelNoHome Val=""/>
					<ProposerPolicyholder_TelNoWork Val=""/>
					<ProposerPolicyholder_EMail Val=""/>
					<ProposerPolicyholder_PostCodeFull Val="AL6 0BH"/>
					<ProposerPolicyholder_PostCodeSector Val="AL6 0"/>
					<ProposerPolicyholder_MaritalStatus Val="S"/>
					<ProposerPolicyholder_HomeownerInd Val="Y"/>
					<ProposerPolicyholder_TitleCode Val="004"/>
					<ProposerPolicyholder_Sex Val="F"/>
					<ProposerPolicyholder_NoOfVehiclesAvailableToFamily Val="1"/>
				</ProposerPolicyholder>
				<Vehicle>
					<Vehicle_VehiclePrn Val="1"/>
					<Vehicle_Model Val="17562302"/>
					<Vehicle_TypeOfFuel Val="002"/>
					<Vehicle_Value Val="4500"/>
					<Vehicle_CubicCapacity Val="1798"/>
					<Vehicle_NoOfSeats Val="5"/>
					<Vehicle_TransmissionType Val="002"/>
					<Vehicle_BodyType Val="03"/>
					<Vehicle_DateFirstRegd Val="01/01/2005"/>
					<Vehicle_YearManufactured Val="2005"/>
					<Vehicle_FirstRegdYear Val="2005"/>
					<Vehicle_PurchaseDate Val="27/05/2014"/>
					<Vehicle_LeftOrRightHandDrive Val="R"/>
					<Vehicle_LocationKeptOvernight Val="3"/>
					<Vehicle_PostCodeSector Val="AL6 0"/>
					<Vehicle_PostCodeFull Val="AL6 0BH"/>
					<Vehicle_AccessoriesValue Val="0"/>
					<Vehicle_ModifiedInd Val="N"/>
					<Vehicle_PersonalImportInd Val="N"/>
					<Vehicle_RegNo Val="XXX999X"/>
					<Vehicle_Keeper Val="1"/>
					<Vehicle_Ownership Val="1"/>
					<Vehicle_DrivenByDriver1 Val="M"/>
					<Vehicle_SecurityDeviceFittedInd Val="N"/>
					<Vehicle_QPlateInd Val="N"/>
					<Vehicle_AnnualMileage Val="12000"/>
					<Vehicle_MileometerReading Val="50000"/>
					<Ncd>
						<Ncd_ClaimedYears Val="5"/>
						<Ncd_ClaimedYearsEarned Val="5"/>
						<Ncd_ClaimedEntitlementReason Val="11"/>
						<Ncd_ClaimedProtectionReqdInd Val="N"/>
						<Ncd_ClaimedPreviousPolicyExpiryDate Val="08/10/2016"/>
					</Ncd>
					<Uses>
						<Uses_AbiCode Val="04"/>
						<Uses_UsedByDriver1Ind Val="Y"/>
					</Uses>
					<DrivenBy>
						<DrivenBy_DriverNumber Val="1"/>
						<DrivenBy_DrivingFrequency Val="M"/>
					</DrivenBy>
				</Vehicle>
				<Cover>
					<Cover_VolXsAmt Val="00"/>
					<Cover_Code Val="01"/>
					<Cover_StartDate Val="08/10/2016"/>
					<Cover_Period Val="12"/>
					<Cover_PeriodUnits Val="2"/>
					<Cover_RequiredDrivers Val="1"/>
					<Cover_DriversAllowed Val="1"/>
					<Cover_VehiclePrn Val="1"/>
				</Cover>
				<Policy>
					<Policy_EffectiveStartDate Val="08/10/2016"/>
					<Policy_InceptionDate Val="08/10/2016"/>
					<Policy_EffectiveStartTime Val="080000"/>
					<Policy_IntermedPolicyRefNo Val="FLANAU1"/>
					<Policy_PrevlyInsdInd Val="Y"/>
					<Policy_PrevInsr Val="38"/>
					<Policy_PrevExpiryDate Val="08/10/2016"/>
					<Policy_PrevPaymentRegularity Val="01"/>
					<Policy_PrevPolicyNo Val="dfgh"/>
					<Policy_EffectiveEndDate Val="08/10/2017"/>
					<Policy_ExpiryDate Val="08/10/2017"/>
					<Policy_EffectiveEndTime Val="235959"/>
					<Policy_RenewalFrequency Val="12"/>
				</Policy>
				<Driver>
					<Driver_Prn Val="1"/>
					<Driver_LicenceCountryOfIssue Val="023"/>
					<Driver_TypeOfDwelling Val="01"/>
					<Driver_RelationshipToProposer Val="P"/>
					<Driver_Title Val="004"/>
					<Driver_Sex Val="F"/>
					<Driver_DateOfBirth Val="01/01/1970"/>
					<Driver_MaritalStatus Val="S"/>
					<Driver_SmokerInd Val="N"/>
					<Driver_AnnualMileage Val="12000"/>
					<Driver_LicenceType Val="F"/>
					<Driver_LicenceDate Val="27/05/1990"/>
					<LicenceRestrictions>
						<LicenceRestrictions_Code Val="8"/>
					</LicenceRestrictions>
					<Driver_RefusedCoverInd Val="N"/>
					<Driver_PrevRestrictiveTermsAppliedInd Val="N"/>
					<Driver_NoOfOtherVehiclesDriven Val="0"/>
					<Driver_ForenameInitial1 Val="Audrey"/>
					<Driver_Surname Val="Bouvier Des Flandres"/>
					<Driver_DrivesVehicle1Ind Val="M"/>
					<Driver_MedicalConditionInd Val="N"/>
					<Driver_ClaimsInd Val="N"/>
					<Driver_ConvictionsInd Val="N"/>
					<Driver_ProsecutionPendingInd Val="N"/>
					<Driver_EverHadPolicyCancelledInd Val="N"/>
					<Driver_PrevlyAppliedIncrsdPremInd Val="N"/>
					<Driver_ResidentOutsideUkInd Val="N"/>
					<Driver_UkResidencyDate Val="01/12/1970"/>
					<Driver_PermanentlyResidentInd Val="Y"/>
					<Driver_NoOfYearsUkResidency Val="43"/>
					<Driver_RegdDisabledInd Val="N"/>
					<Driver_DisabledBadgeHolderInd Val="N"/>
					<Driver_NonMotoringConvictionInd Val="N"/>
					<Driver_OtherVehicleOwnedInd Val="N"/>
					<Occupation qs:listtype="S">
						<Occupation_Code Val="001" qs:desc="Abattoir Worker"/>
						<Occupation_EmployersBusiness Val="188" qs:desc="Abattoir"/>
						<Occupation_EmploymentType Val="E" qs:desc="Employed"/>
						<Occupation_FullTimeEmploymentInd Val="Y" qs:desc="Yes"/>
					</Occupation>
					<DrivesVehicle>
						<DrivesVehicle_DrivingFrequency Val="M"/>
					</DrivesVehicle>
				</Driver>
				<Software>
					<Software_Supplier Val="39"/>
				</Software>
				<Intermediary>
					<Intermediary_Code Val="C00074"/>
					<Intermediary_BusinessSourceCode Val="002"/>
					<Intermediary_DataSourceCode Val="002"/>
					<Intermediary_QuoteArea Val="02"/>
				</Intermediary>
				<Miscellaneous>
					<Miscellaneous_TodaysDate Val="06/10/2016"/>
				</Miscellaneous>
			</PolData>
		</PolMessage>
	</ISBody>
</ISMessage>```


