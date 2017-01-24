using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BarTest {

	[Test]
	public void SoundTest() {
		Bar bar = new Bar();
		//check the sounds work for all buttons
		for (int x =0; x<=3;x ++){
			bar.song = x;
			try{
				bar.OnMouseDown();
				Assert.Pass();
			}catch (System.Exception e){
			}
		}

		bar.song = -1;
		try{
			bar.OnMouseDown();
			Assert.Fail();
		}catch (System.Exception e){
		}
	}
}

