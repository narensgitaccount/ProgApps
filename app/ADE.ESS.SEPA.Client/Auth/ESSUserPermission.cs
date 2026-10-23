namespace ADE.ESS.SEPA.Client.Auth
{
    public class ESSUserPermission : System.IEquatable<ESSUserPermission>
    {
        public int? EntityId { get; set; }

        public ESSUserPermissionsEnum Permission { get; set; }

        public bool Equals(ESSUserPermission other)
        {
            throw new System.NotImplementedException();
        }
    }
}
