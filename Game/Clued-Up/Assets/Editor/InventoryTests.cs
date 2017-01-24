using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class InventoryTests {
	Story testStory;
	Character testCharacter;
	Clue testClue;
	Detective testDetective;
	Inventory testInventory;

	[TestFixtureSetUp]
	public void TestSetup(){
		testStory = new Story();
		testStory.setStory();
		testClue = testStory.getCluesInRoom(0)[0].GetComponent<Clue>();
		testInventory = new Inventory();
	}
		
	[Test]
	public void CollectClueTest(){
		try{
			testInventory.collect(testClue);
			Assert.Pass();
		} catch (Exception e ){}
		Assert.True(testInventory.isCollected(testClue.name));

		//now try and add clue again
		try{
			testInventory.collect(testClue);
			Assert.Fail();
		} catch (Exception e ){}

		//now test the clue is collected
		Assert.IsTrue(testInventory.isCollected(testClue.name));

	}

	[Test]
	public void EncounterTest(){
		try{
			testInventory.encounter(testStory.randomAliveCharacter().GetComponent<Character>());
			Assert.Pass();
		}catch (Exception e){
		}
	}

}
