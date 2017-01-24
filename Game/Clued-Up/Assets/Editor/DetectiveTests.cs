using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class DetectiveTests {
	Detective testDetective;

	[TestFixtureSetUp]
	public void testSetup(){
		testDetective = new Detective();

	}

	[Test]
	public void addVisitedRoomTest(){
		testDetective.addVisitedRoom(0);
		Assert.True(testDetective.isVisited(0));
	}

	[Test]
	public void walkInTest(){
		for (int x = 0; x < 2; x ++){
			testDetective.walkInDirection = x;
			try{
				testDetective.walkIn();
				Assert.Pass();
			}catch (Exception e){
			}

		}
	}
}
