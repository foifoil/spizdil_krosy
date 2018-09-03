namespace EveAIO.Cloudflare
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ChallengeSolution : IEquatable<ChallengeSolution>
    {
        public ChallengeSolution(string clearancePage, string verificationCode, string pass, double answer)
        {
            Class7.RIuqtBYzWxthF();
            this.<ClearancePage>k__BackingField = clearancePage;
            this.<VerificationCode>k__BackingField = verificationCode;
            this.<Pass>k__BackingField = pass;
            this.<Answer>k__BackingField = answer;
        }

        public string ClearancePage =>
            this.<ClearancePage>k__BackingField;
        public string VerificationCode =>
            this.<VerificationCode>k__BackingField;
        public string Pass =>
            this.<Pass>k__BackingField;
        public double Answer =>
            this.<Answer>k__BackingField;
        public string ClearanceQuery =>
            $"{this.ClearancePage}?jschl_vc={this.VerificationCode}&pass={this.Pass}&jschl_answer={this.Answer.ToString("R", CultureInfo.InvariantCulture)}";
        public static bool operator !=(ChallengeSolution solutionA, ChallengeSolution solutionB) => 
            !solutionA.Equals(solutionB);

        public override bool Equals(object obj)
        {
            ChallengeSolution? nullable = obj as ChallengeSolution?;
            if (!nullable.HasValue)
            {
                return false;
            }
            return this.Equals(nullable.Value);
        }

        public override int GetHashCode() => 
            this.ClearanceQuery.GetHashCode();

        public bool Equals(ChallengeSolution other) => 
            (other.ClearanceQuery == this.ClearanceQuery);
    }
}

