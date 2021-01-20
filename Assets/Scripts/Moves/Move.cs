namespace Moves {
	public interface Move {
		bool Freezed { get; set; }
		void execute();
	}
}