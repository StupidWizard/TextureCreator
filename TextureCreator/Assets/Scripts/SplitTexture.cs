using UnityEngine;
using System.Collections;

public enum Rotate {
	ROT_0 = 0,
	ROT_90,
	ROT_180,
	ROT_270
}


public class Cell {

	public Vec2Int startPoint;
	public Vec2Int endPoint;

	public Cell() {
		startPoint = new Vec2Int();
		endPoint = new Vec2Int(1, 1);
	}

	public Cell(Vec2Int startPoint, Vec2Int endPoint) {
		this.startPoint = startPoint;
		this.endPoint = endPoint;
	}
}

public class Vec2Int {

	public int x = 0;
	public int y = 0;

	public Vec2Int() {
		
	}

	public Vec2Int(int x, int y) {
		this.x = x;
		this.y = y;
	}
}

public class SplitTexture : MonoBehaviour {	

	[SerializeField]
	Texture2D originTexture;

	[SerializeField]
	int nCol = 1;

	[SerializeField]
	int nRow = 1;

	[SerializeField]
	Rotate rotType = Rotate.ROT_0;

	[SerializeField]
	bool flipX = false;

	[SerializeField]
	bool flipY = false;

	[SerializeField]
	string nameOut;

	[SerializeField]
	int paddingLeft;

	[SerializeField]
	int paddingRight;

	[SerializeField]
	int paddingTop;

	[SerializeField]
	int paddingBotton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	[ContextMenu("RotateAllFrame")]
	void RotateAllFrame() {

		Vec2Int size = new Vec2Int (originTexture.width / nCol, originTexture.height / nRow);
		Debug.LogError("Size cell = (" + size.x + ", " + size.y + ")");

		int width = originTexture.width;
		int height = originTexture.height;
		if (rotType == Rotate.ROT_90 || rotType == Rotate.ROT_270) {
			
			width = size.y * nCol;
			height = size.x * nRow;

			Debug.LogError("rotate size from (" + originTexture.width + ", " + originTexture.height + ") to " + " (" + width + ", " + height + ")");
		}
		Texture2D targetTexture = new Texture2D(width, height);
		Vec2Int sizeTarget = new Vec2Int(targetTexture.width / nCol, targetTexture.height / nCol);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				targetTexture.SetPixel(i, j, Color.clear);
			}
		}

		for (int i = 0; i < nCol; i++) {
			for (int j = 0; j < nRow; j++) {
				// cell[j, i]
				Vec2Int oriO = new Vec2Int(i * size.x, j * size.y);
				Vec2Int tarO = new Vec2Int(i * sizeTarget.x, j * sizeTarget.y);
				TextureUtils.CopyRotateCell(originTexture, targetTexture, oriO, tarO, size, rotType);
			}
		}

		string pathSave = System.IO.Path.Combine(Application.dataPath, nameOut + ".png");

		System.IO.File.WriteAllBytes(pathSave, targetTexture.EncodeToPNG());

	}
		
	[ContextMenu("Flip Image")]
	void FlipAllFrame() {

		Vec2Int size = new Vec2Int (originTexture.width / nCol, originTexture.height / nRow);
		Debug.LogError("Size cell = (" + size.x + ", " + size.y + ")");

		int width = originTexture.width;
		int height = originTexture.height;

		Texture2D targetTexture = new Texture2D(width, height);
		Vec2Int sizeTarget = new Vec2Int(targetTexture.width / nCol, targetTexture.height / nCol);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				targetTexture.SetPixel(i, j, Color.clear);
			}
		}

		for (int i = 0; i < nCol; i++) {
			for (int j = 0; j < nRow; j++) {
				// cell[j, i]
				Vec2Int oriO = new Vec2Int(i * size.x, j * size.y);
				Vec2Int tarO = new Vec2Int(i * sizeTarget.x, j * sizeTarget.y);
				TextureUtils.CopyFlipCell(originTexture, targetTexture, oriO, tarO, size, flipX, flipY);
			}
		}

		string pathSave = System.IO.Path.Combine(Application.dataPath, nameOut + ".png");

		System.IO.File.WriteAllBytes(pathSave, targetTexture.EncodeToPNG());
	}


	[ContextMenu("ExtendImage")]
	void ExtendFrame() {
		Vec2Int size = new Vec2Int (originTexture.width / nCol, originTexture.height / nRow);
		Debug.LogError("Size cell = (" + size.x + ", " + size.y + ")");



		Vec2Int sizeTarget = new Vec2Int(size.x + paddingLeft + paddingRight, size.y + paddingTop + paddingBotton);
		Texture2D targetTexture = new Texture2D(sizeTarget.x * nCol, sizeTarget.y * nRow);

		int width = targetTexture.width;
		int height = targetTexture.height;

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				targetTexture.SetPixel(i, j, Color.clear);
			}
		}

		Vec2Int offset = new Vec2Int(paddingLeft, paddingTop);
		for (int i = 0; i < nCol; i++) {
			for (int j = 0; j < nRow; j++) {
				// cell[j, i]
				Vec2Int oriO = new Vec2Int(i * size.x, j * size.y);
				Vec2Int tarO = new Vec2Int(i * sizeTarget.x, j * sizeTarget.y);
				Cell oriData = new Cell(oriO, new Vec2Int(oriO.x + size.x - 1, oriO.y + size.y - 1));
				Cell tarData = new Cell(tarO, new Vec2Int(tarO.x + sizeTarget.x - 1, tarO.y + sizeTarget.y - 1));
				TextureUtils.CopyExtendCell(originTexture, oriData,
					targetTexture, tarData, offset);
			}
		}

		string pathSave = System.IO.Path.Combine(Application.dataPath, nameOut + ".png");

		System.IO.File.WriteAllBytes(pathSave, targetTexture.EncodeToPNG());
	}





}
