using UnityEngine;
using System.Collections;

/// <summary>
/// Texture utils.
/// 
/// </summary>
public class TextureUtils {


	/// <summary>
	/// Copies pixels from 1 cell of origin to 1 cell of target and flip it.
	/// </summary>
	/// <param name="origin">Origin.</param>
	/// <param name="target">Target.</param>
	/// <param name="originO">Origin o.</param>
	/// <param name="targetO">Target o.</param>
	/// <param name="size">Size.</param>
	/// <param name="flipX">If set to <c>true</c> flip x.</param>
	/// <param name="flipY">If set to <c>true</c> flip y.</param>
	public static void CopyFlipCell(Texture2D origin, Texture2D target, Vec2Int originO, Vec2Int targetO, 
		Vec2Int size, bool flipX, bool flipY) {
		int ORI_Y_MAX = origin.height - 1;
		int TAR_Y_MAX = target.height - 1;// - targetO.y;

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



	/// <summary>
	/// Copies pixels from 1 cell of origin to 1 cell of target (and extend target cell). 
	/// offset is pos(O) of target - pos(O) of origin.
	/// </summary>
	/// <param name="origin">Origin texture.</param>
	/// <param name="originData">Origin data (start and end point).</param>
	/// <param name="target">Target.</param>
	/// <param name="targetData">Target data.</param>
	/// <param name="offset">Offset = pos(O) of target - pos(O) of origin.</param>
	public static void CopyExtendCell(Texture2D origin, Cell originData,
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
	/// Copies FULL pixels from origin to target and rotate. offset if center of cell
	/// </summary>
	/// <param name="origin">Origin Texture.</param>
	/// <param name="target">Target Texture.</param>
	/// <param name="originO">root, point O of origin.</param>
	/// <param name="targetO">root, point O of target.</param>
	/// <param name="size">Size.</param>
	/// <param name="rotType">type of rotation: 0-90-180-270.</param>
	public static void CopyRotateCell(Texture2D origin, Texture2D target, Vec2Int originO, Vec2Int targetO, Vec2Int size, Rotate rotType) {
		int ORI_Y_MAX = origin.height - 1;
		int TAR_Y_MAX = target.height - 1;

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
}
