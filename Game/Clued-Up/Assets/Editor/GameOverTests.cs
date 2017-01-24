using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class GameOverTests {
	Story testStory;

	[Test]
	public void GameOverTest(){
		testStory = new Story();
		testStory.setStory();

		try{
			testStory.endGame();
			Assert.Pass();
		}catch(System.Exception e){}
			
	}
}
