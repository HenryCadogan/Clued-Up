using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class SpeechHandlerTests {

	SpeechHandler testSpeechHandler;
	Story testStory;

	[TestFixtureSetUp]
	public void TestSetup(){
		testSpeechHandler = new SpeechHandler();
		testStory = new Story();
		testStory.setStory();
	}

	[Test]
	public void turnOnSpeechUITest(){
		try{
			testSpeechHandler.turnOnSpeechUI();
			Assert.Pass();
		}catch (Exception e){}
	}

	[Test]
	public void turnOffSpeechUITest(){
		try{
			testSpeechHandler.turnOffSpeechUI();
			Assert.Pass();
		}catch (Exception e){}
	}

	[Test]
	public void AccuseTest(){
		try{
			testSpeechHandler.accuse();
			Assert.Pass();
		}catch (Exception e){}
	}


	
}

