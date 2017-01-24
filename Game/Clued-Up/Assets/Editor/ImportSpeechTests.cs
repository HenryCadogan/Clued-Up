using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class ImportSpeechTests {
	Story testStory;
	ImportSpeech testImportSpeech;

	[TestFixtureSetUp]
	public void TestSetup(){
		testStory = new Story();
		testStory.setStory();
	}


	[Test]
	public void ActualStartTest(){
		
		try{
			testImportSpeech.actualStart();
			Assert.Pass();
		}catch (Exception e){
		}
	}


	[Test]
	public void NextLineTest(){
		try{
			testImportSpeech.nextLine();
			Assert.Pass();
		}catch (Exception e){
		}
	}

	[Test]
	public void SetBranchTest(){
		try {
			testImportSpeech.setBranch("");
			Assert.Fail();
		}catch (Exception e){}
	}

}
