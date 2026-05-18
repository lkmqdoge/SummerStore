{
  description = "Empty flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
  };

  outputs = { self, nixpkgs }:
  let
    inherit (nixpkgs) lib;
    forAllSystems = lib.genAttrs lib.systems.flakeExposed;
  in
  {
    devShells = forAllSystems (
      system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config.allowUnfree = true;
        };
      in
      {
        default = pkgs.mkShell {
          buildInputs = with pkgs; [
            # jetbrains.rider
            pnpm
          ];
        };
      }
    );
  };
}
