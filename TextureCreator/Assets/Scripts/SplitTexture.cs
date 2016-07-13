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
				CopyPixels(originTexture, targetTexture, oriO, tarO, size, rotType);
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
				CopyPixels(originTexture, targetTexture, oriO, tarO, size, flipX, flipY);
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
				CopyPixels(originTexture, oriData,
					targetTexture, tarData, offset);
			}
		}

		string pathSave = System.IO.Path.Combine(Application.dataPath, nameOut + ".png");

		System.IO.File.WriteAllBytes(pathSave, targetTexture.EncodeToPNG());
	}

	/// <summary>
	/// Copies pixels from 1 cell of origin to 1 cell of target. offset is pos(O) of target - pos(O) of origin.
	/// </summary>
	/// <param name="origin">Origin texture.</param>
	/// <param name="originData">Origin data (start and end point).</param>
	/// <param name="target">Target.</param>
	/// <param name="targetData">Target data.</param>
	/// <param name="offset">Offset = pos(O) of target - pos(O) of origin.</param>
	void CopyPixels(Texture2D origin, Cell originData,
		Texture2D target, Cell targetData, Vec2Int offset) {
		Vec2Int oriSize = new Vec2Int(originData.endPoint.x - originData.startPoint.x + 1, originData.endPoint.y - originData.startPoint.y + 1);
		Vec2Int tarSize = new Vec2Int(targetData.endPoint.x - targetData.startPoint.x + 1, targetData.endPoint.y - targetData.startPoint.y + 1);

		int ORI_Y_MAX = origin.height - 1;
		int TAR_Y_MAX = target.height - 1;

		for (int dX = 0; dX < oriSize.x; dX++) {
			for (int dY = 0; dY < oriSize.y; dY++) {
				int dtX = dX + offset.x;
				int dtY = dY + offset.y;
				if ((0 <= dtX && dtX < tarSize.x)
					&& (0 <= dtY && dtY < tarSize.y)) {
					int oriX = originData.startPoint.x + dX;
					int oriY = originData.startPoint.y + dY;
					int tarX = targetData.startPoint.x + dtX;
					int tarY = targetData.startPoint.y + dtY;
					target.SetPixel(tarX, TAR_Y_MAX - tarY, origin.GetPixel(oriX, ORI_Y_MAX - oriY));
				}
			}
		}

		target.Apply();
	}


	/// <summary>
	/// Copies FULL pixels from origin to target with rotate. NOT HAVE OFFSET
	/// </summary>
	/// <param name="origin">Origin Texture.</param>
	/// <param name="target">Target Texture.</param>
	/// <param name="originO">root, point O of origin.</param>
	/// <param name="targetO">root, point O of target.</param>
	/// <param name="size">Size.</param>
	/// <param name="rotType">type of rotation: 0-90-180-270.</param>
	void CopyPixels(Texture2D origin, Texture2D target, Vec2Int originO, Vec2Int targetO, Vec2Int size, Rotate rotType) {
		int ORI_Y_MAX = origin.height - 1;
		int TAR_Y_MAX = target.height - 1 - targetO.y;

		switch (rotType) {
		case Rotate.ROT_90:
			for (int dX = 0; dX < size.x; dX++) {
				for (int dY = 0; dY < size.y; dY++) {
					target.SetPixel(targetO.x + size.y - 1 - dY, TAR_Y_MAX - (targetO.y + dX),
						origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
				}
			}
			break;


		case Rotate.ROT_180:
			for (int dX = 0; dX < size.x; dX++) {
				for (int dY = 0; dY < size.y; dY++) {
					target.SetPixel(targetO.x + size.x - 1 - dX, TAR_Y_MAX - (targetO.y + size.y - 1- dY),
						origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
				}
			}
			break;

		case Rotate.ROT_270:
			for (int dX = 0; dX < size.x; dX++) {
				for (int dY = 0; dY < size.y; dY++) {
					target.SetPixel(targetO.x + dY, TAR_Y_MAX - (targetO.y + size.x - 1 - dX),
						origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
				}
			}
			break;

		default:
			for (int dX = 0; dX < size.x; dX++) {
				for (int dY = 0; dY < size.y; dY++) {
					target.SetPixel(targetO.x + dX, TAR_Y_MAX - (targetO.y + dY),
						origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
				}
			}
			break;
		}

		target.Apply();
	}


	void CopyPixels(Texture2D origin, Texture2D target, Vec2Int originO, Vec2Int targetO, Vec2Int size, bool flipX, bool flipY) {
		int ORI_Y_MAX = origin.height - 1;
		int TAR_Y_MAX = target.height - 1 - targetO.y;

		if (flipX) {
			if (flipY) {
				for (int dX = 0; dX < size.x; dX++) {
					for (int dY = 0; dY < size.y; dY++) {
						target.SetPixel(targetO.x + size.x - 1 - dX, TAR_Y_MAX - (targetO.y + size.y - 1 - dY),
							origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
					}
				}
			} else {
				for (int dX = 0; dX < size.x; dX++) {
					for (int dY = 0; dY < size.y; dY++) {
						target.SetPixel(targetO.x + size.x - 1 - dX, TAR_Y_MAX - (targetO.y + dY),
							origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
					}
				}
			}
		} else {
			if (flipY) {
				for (int dX = 0; dX < size.x; dX++) {
					for (int dY = 0; dY < size.y; dY++) {
						target.SetPixel(targetO.x + dX, TAR_Y_MAX - (targetO.y + size.y - 1 - dY),
							origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
					}
				}
			} else {
				for (int dX = 0; dX < size.x; dX++) {
					for (int dY = 0; dY < size.y; dY++) {
						target.SetPixel(targetO.x + dX, TAR_Y_MAX - (targetO.y + dY),
							origin.GetPixel(originO.x + dX, ORI_Y_MAX - (originO.y + dY)));
					}
				}
			}
		}

		target.Apply();
	}
}
