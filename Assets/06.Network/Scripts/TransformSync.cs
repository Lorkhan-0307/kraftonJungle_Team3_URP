using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TransformSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 latestPos; // ��Ʈ��ũ���� ����ȭ�� ��ġ
    private Quaternion latestRot; // ��Ʈ��ũ���� ����ȭ�� ȸ��

    // ��Ʈ��ũ ����ȭ�� ���� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ���� �����͸� ������ ��
        {
            stream.SendNext(transform.position); // ��ġ ����
            stream.SendNext(transform.rotation); // ȸ�� ����
        }
        else // �ٸ� �÷��̾��� �����͸� ���� ��
        {
            latestPos = (Vector3)stream.ReceiveNext(); // ��ġ �ޱ�
            latestRot = (Quaternion)stream.ReceiveNext(); // ȸ�� �ޱ�
        }
    }
    private void Update()
    {
        if (!photonView.IsMine) // �ٸ� Ŭ���̾�Ʈ�� ������Ʈ�� ��
        {
            // ��ġ�� ȸ���� �ε巴�� ������Ʈ
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10);
        }
    }
}
