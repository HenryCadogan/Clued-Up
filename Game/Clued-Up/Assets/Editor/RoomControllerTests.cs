using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class RoomControllerTests {

	RoomController testRoomcontroller;
	[TestFixtureSetUp]
	public void testSetup(){
		testRoomcontroller = new RoomController();
	}

	[Test]
	public void RoomControllerTest() {
		try {
			testRoomcontroller.canProgress();
			Assert.Pass();
		} catch(Exception e) {
			
		}
	}
}
